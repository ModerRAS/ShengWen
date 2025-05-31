const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

// 计算绝对路径
const buildDir = path.resolve(__dirname, 'build');
const targetDir = path.resolve(__dirname, '../ShengWen.Server/wwwroot');

try {
  // 清空目标目录
  if (fs.existsSync(targetDir)) {
    const files = fs.readdirSync(targetDir);
    for (const file of files) {
      const curPath = path.join(targetDir, file);
      fs.rmSync(curPath, { recursive: true, force: true });
    }
  } else {
    fs.mkdirSync(targetDir, { recursive: true });
  }

  // 复制构建文件
  fs.cpSync(buildDir, targetDir, { recursive: true });
  
  console.log('部署成功！');
} catch (error) {
  console.error('部署失败:', error);
  process.exit(1);
}