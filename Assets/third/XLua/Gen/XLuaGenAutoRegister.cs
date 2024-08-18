#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Tutorial.BaseClass), TutorialBaseClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.TestEnum), TutorialTestEnumWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClass), TutorialDerivedClassWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.ICalc), TutorialICalcWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClassExtensions), TutorialDerivedClassExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaBehaviour), CoreLuaBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaBeginDragEventTrigger), CoreLuaBeginDragEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaButtonEventTrigger), CoreLuaButtonEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaDragEventTrigger), CoreLuaDragEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaDropEventTrigger), CoreLuaDropEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaEndDragEventTrigger), CoreLuaEndDragEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaPointerClickEventTrigger), CoreLuaPointerClickEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaPointerDownEventTrigger), CoreLuaPointerDownEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaPointerEnterEventTrigger), CoreLuaPointerEnterEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaPointerExitEventTrigger), CoreLuaPointerExitEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaPointerUpEventTrigger), CoreLuaPointerUpEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaScrollEventTrigger), CoreLuaScrollEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaToggleEventTrigger), CoreLuaToggleEventTriggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Core.LuaButtonScript), CoreLuaButtonScriptWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Tutorial.DerivedClass.TestEnumInner), TutorialDerivedClassTestEnumInnerWrap.__Register);
        
        
        
        }
        
        public static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            
            translator.AddInterfaceBridgeCreator(typeof(Tutorial.CSCallLua.ItfD), TutorialCSCallLuaItfDBridge.__Create);
            
        }
	}
}

namespace XLua
{
	internal partial class InternalGlobals_Gen
    {
	    
        private delegate bool TryArrayGet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int index);
        private delegate bool TryArraySet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int array_idx, int obj_idx);
	    private static void Init(
            out Dictionary<Type, IEnumerable<MethodInfo>> extensionMethodMap,
            out TryArrayGet genTryArrayGetPtr,
            out TryArraySet genTryArraySetPtr)
		{
            XLua.LuaEnv.AddIniter(XLua.CSObjectWrap.XLua_Gen_Initer_Register__.Init);
            XLua.LuaEnv.AddIniter(XLua.ObjectTranslator_Gen.Init);
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
			
            genTryArrayGetPtr = StaticLuaCallbacks_Wrap.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks_Wrap.__tryArraySet;
		}
	}
}
