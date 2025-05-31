import React, { useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import NavMenu from '../components/NavMenu';
import { initializeIcons } from '@fluentui/react';
import '@fluentui/react/dist/css/fabric.min.css';

const MainLayout: React.FC = () => {
  useEffect(() => {
    initializeIcons();
  }, []);

  return (
    <div className="main-layout">
      <NavMenu />
      <div className="content">
        <Outlet />
      </div>
    </div>
  );
};

export default MainLayout;