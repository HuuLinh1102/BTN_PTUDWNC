import React from "react";
import { Navbar as Nb, Nav } from "react-bootstrap";
import {
    Link
} from "react-router-dom";

const Navbar = () => {
    return (
        <Nb collapseOnSelect expanded="sm" bg="white" variant="light" className="border-bottom shadow" >
            <div className="container-fluid">
                <Nb.Brand href="/admin">Traditional GameGuide</Nb.Brand>
                <Nb.Toggle aria-controls="responsive-navbar-nav" />
                <Nb.Collapse id="responsive-navbar-nav" className="d-sm-inline-flex
                justify-contex-betwen">
                    <Nav className='mr-auto flex-grow-1'>
                        <Nav.Item>
                            <Link to="/admin" className='nav-link text-dark'>
                                Trang chủ
                            </Link>
                        </Nav.Item>
                        <Nav.Item>
                            <Link to='/admin/post' className='nav-link text-dark'>
                                Danh sách các trò chơi
                            </Link>
                        </Nav.Item>
                        <Nav.Item>
                            <Link to='/admin/accout' className='nav-link text-dark'>
                                Đăng kí tài khoản
                            </Link>
                        </Nav.Item>
                        <Nav.Item>
                            <Link to='/admin/comment' className='nav-link text-dark'>
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