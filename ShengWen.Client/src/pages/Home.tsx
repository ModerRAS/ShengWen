import React from 'react';
import { Stack, Text } from '@fluentui/react';

const Home: React.FC = () => {
  return (
    <Stack tokens={{ childrenGap: 20 }} styles={{ root: { padding: 20 } }}>
      <Text variant="xxLarge">欢迎使用声文</Text>
      <Text variant="large">这是声文应用的主页</Text>
      <Text>
        声文是一个智能语音处理平台，提供语音识别、文本生成等功能。
      </Text>
    </Stack>
  );
};

export default Home;