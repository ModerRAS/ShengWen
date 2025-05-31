import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import Agents from './pages/Agents';
import ApiKeys from './pages/ApiKeys';
import Home from './pages/Home';
import Login from './pages/Login';

const RequireAuth = ({ children }: { children: React.ReactElement }) => {
  const token = localStorage.getItem('jwtToken');
  return token ? children : <Navigate to="/login" replace />;
};

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/" element={<MainLayout />}>
          <Route index element={
            <RequireAuth>
              <Home />
            </RequireAuth>
          } />
          <Route path="agents" element={
            <RequireAuth>
              <Agents />
            </RequireAuth>
          } />
          <Route path="api-keys" element={
            <RequireAuth>
              <ApiKeys />
            </RequireAuth>
          } />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
