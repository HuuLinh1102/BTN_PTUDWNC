import React, { useEffect } from 'react';

const Accout = () => {
  useEffect(() => {
    document.title = 'Đăng kí tài khoản';
  }, []);

  return (
    <h1>
      Đây là trang đăng kí tài khoản
    </h1>
  );
}

export default Accout;