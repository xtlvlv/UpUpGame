import os
import re
import argparse

parser = argparse.ArgumentParser(description='Update namespace in C# files.')
parser.add_argument('path', type=str, help='替换的路径')
parser.add_argument('name', type=str, help='新的命名空间名，有则替换，无则添加')

args = parser.parse_args()

print('更换路径: ' + args.path)
print('新namespace: ' + args.name)
directory_path = args.path
new_namespace = args.name

namespace_pattern = re.compile(r'^\s*namespace\s+[\w.]+')

def process_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as file:
        content = file.read()

    if namespace_pattern.search(content):
        new_content = namespace_pattern.sub(f'namespace {new_namespace}', content)
    else:
        new_content = f'namespace {new_namespace}\n{{\n{content}\n}}'

    with open(file_path, 'w', encoding='utf-8') as file:
        file.write(new_content)

for subdir, dirs, files in os.walk(directory_path):
    for filename in files:
        filepath = os.path.join(subdir, filename)
        if filepath.endswith('.cs'):  # 确保只处理C#文件
            process_file(filepath)

print('替换命名空间完成')