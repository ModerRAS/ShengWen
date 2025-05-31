import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Nav, Stack, Text, PrimaryButton } from '@fluentui/react';
import type { INavLinkGroup } from '@fluentui/react';

const NavMenu: React.FC = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('jwtToken');
    setIsLoggedIn(!!token);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('jwtToken');
    setIsLoggedIn(false);
    navigate('/login');
  };

  const navLinkGroups: INavLinkGroup[] = [
    {
      links: [
        {
          name: 'Home',
          url: '/',
          key: 'home',
        },
        {
          name: 'Agents',
          url: '/agents',
          key: 'agents',
        },
        {
          name: 'API Keys',
          url: '/api-keys',
          key: 'api-keys',
        },
      ],
    },
  ];

  return (
    <Stack horizontal verticalAlign="center" horizontalAlign="space-between" styles={{ root: { padding: '0 20px', height: 50, backgroundColor: '#f3f2f1', borderBottom: '1px solid #e1dfdd' } }}>
      <Text variant="xLarge" styles={{ root: { fontWeight: 'bold' } }}>
        <Link to="/" style={{ textDecoration: 'none', color: 'inherit' }}>ShengWen</Link>
      </Text>
      
      <Stack horizontal tokens={{ childrenGap: 15 }}>
        <Nav groups={navLinkGroups} styles={{ root: { height: 50 } }} />
        
        {isLoggedIn ? (
          <PrimaryButton onClick={handleLogout} styles={{ root: { marginTop: 10, height: 32 } }}>
            登出
          </PrimaryButton>
        ) : (
          <Link to="/login">
            <PrimaryButton styles={{ root: { marginTop: 10, height: 32 } }}>
              登录
            </PrimaryButton>
          </Link>
        )}
      </Stack>
    </Stack>
  );
};

export default NavMenu;