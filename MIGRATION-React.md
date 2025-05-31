# 前端框架迁移文档

## 迁移概述
本项目已完成从Blazor到React的前端框架迁移，主要变更包括：
- 移除Blazor相关文件（.razor, .cshtml等）
- 新增React客户端项目（基于TypeScript + Vite）
- 重构后端以支持React静态文件服务

## 技术栈变更
| 组件         | 原方案       | 新方案               |
|--------------|-------------|---------------------|
| 前端框架     | Blazor      | React 18           |
| 构建工具     | .NET        | Vite               |
| 语言         | C#          | TypeScript         |
| 路由         | Blazor路由  | React Router v7    |

## 后端配置变更
在`Program.cs`中添加静态文件服务配置：
```csharp
var clientAppPath = Path.Combine(builder.Environment.ContentRootPath, "../ShengWen.Client/dist");
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(clientAppPath)
});
```

## 前端项目结构
```
ShengWen.Client/
├── dist/                  # 构建输出目录
├── public/                # 公共资源
├── src/
│   ├── components/        # 通用组件
│   ├── layouts/           # 页面布局
│   ├── pages/             # 页面组件
│   ├── App.tsx            # 应用入口
│   └── main.tsx           # 渲染入口
```

## 构建和运行指南
1. 构建前端：
```bash
cd ShengWen.Client
npm install
npm run build
```

2. 启动后端：
```bash
cd ShengWen.Server
dotnet run
```

3. 访问应用：
http://localhost:5000

## 注意事项
1. 开发时使用前端热更新：
```bash
cd ShengWen.Client
npm run dev
```

2. 后端可能显示HTTPS重定向警告（不影响功能）：
```
warn: Failed to determine the https port for redirect.