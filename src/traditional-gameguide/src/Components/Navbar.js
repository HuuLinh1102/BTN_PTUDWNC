import React from "react";
import { Navbar as Nb, Nav } from "react-bootstrap";
import {
  Link
} from 'react-router-dom';

const Navbar = () => {
  return (
    <Nb collapseOnSelect expand='sm' bg='white' variant='light' className='border-bottom shadow'>
      <div className='container-fluid'>
        <Nb.Brand href='/'>Traditional GameGuide</Nb.Brand>
        <Nb.Toggle aria-controls='responsive-navbar-nav' />
        <Nb.Collapse id='responsive-navbar-nv' className='d-sm-inline-flex
        justify-content-between'>
          <Nav className='mr-auto flex-grow-1'>
            <Nav.Item>
              <Link to='/' className='nav-link text-dark'>
                Trang chủ
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link to='/blog/post' className='nav-link text-dark'>
                Danh sách các trò chơi
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link to='/blog/accout' className='nav-link text-dark'>
                Đăng kí tài khoản
              </Link>
            </Nav.Item>
            <Nav.Item>
              <Link to='/blog/comment' className='nav-link text-dark'>
                Bình luận
              </Link>
            </Nav.Item>
          </Nav>
        </Nb.Collapse>
      </div>
    </Nb>
  )
}

export default Navbar;