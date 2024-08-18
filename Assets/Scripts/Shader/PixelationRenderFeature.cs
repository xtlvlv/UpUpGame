using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelationRenderFeature : ScriptableRendererFeature
{
    class PixelationPass : ScriptableRenderPass
    {
        public Material pixelationMaterial;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public PixelationPass(Material material)
        {
            pixelationMaterial = material;
            tempTexture.Init("_TempTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // 在 OnCameraSetup 中获取 cameraColorTarget
            source = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Pixelation Effect");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);
            Blit(cmd, source, tempTexture.Identifier(), pixelationMaterial);
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    public Material pixelationMaterial;
    PixelationPass pixelationPass;

    public override void Create()
    {
        pixelationPass = new PixelationPass(pixelationMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // 不再直接访问 cameraColorTarget
        renderer.EnqueuePass(pixelationPass);
    }
}
