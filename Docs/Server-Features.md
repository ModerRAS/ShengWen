# 服务端功能文档

## 概述
服务端主要提供任务管理、代理状态监控和API密钥管理功能，语音处理功能已迁移到客户端。

## 功能详情

### 1. 任务管理
- **创建任务**  
  `POST /api/tasks`  
  请求参数: audioUrl (音频文件URL)  
  返回: 创建的任务对象

- **获取下一个任务**  
  `GET /api/tasks/next`  
  返回: 下一个待处理任务或404

- **完成任务**  
  `POST /api/tasks/complete`  
  请求参数: taskId, transcript  
  返回: 200 OK

- **任务状态**  
  - Pending: 待处理
  - Processing: 处理中
  - Completed: 已完成
  - Failed: 失败

### 2. 代理状态监控
- **获取代理状态**  
  `GET /api/agents/status`  
  返回: 所有代理状态列表  
  状态判断:
  - 最后活跃时间<1分钟: 在线
  - 1-5分钟: 忙碌
  - >5分钟: 离线

### 3. API密钥管理
- **创建密钥**  
  `POST /api/apikeys`  
  请求参数: name  
  返回: 创建的API密钥对象

- **获取所有密钥**  
  `GET /api/apikeys`  
  返回: API密钥列表

- **删除密钥**  
  `DELETE /api/apikeys/{id}`  
  返回: 204 No Content

## 技术实现
- **任务队列**: 使用ConcurrentQueue实现
- **代理状态**: 使用Redis存储
- **API密钥**: 内存存储(ConcurrentDictionary)
- **语音处理**: 已迁移到客户端

## Agent实现

### 核心功能
1. **语音识别处理**:
   - 使用Whisper.net进行语音转文字
   - 支持生成SRT字幕格式
   - 自动下载Whisper模型(如果本地不存在)

2. **任务处理流程**:
   - 轮询获取待处理任务(GET /api/tasks/next)
   - 下载音频文件
   - 生成SRT字幕
   - 推送结果到服务端(POST /api/tasks/complete)

3. **并发控制**:
   - 最大并发处理数: 2
   - 使用SemaphoreSlim控制并发

4. **认证机制**:
   - 使用JWT令牌认证
   - 默认用户名/密码: admin/password

### 技术细节
- 模型: ggml-large-v3-q5_0.bin
- 支持语言: 自动检测(auto)
- 重试机制: 任务失败后5秒重试
- 无任务时: 10秒后重试查询
