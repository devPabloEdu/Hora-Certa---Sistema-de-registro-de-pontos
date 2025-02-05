import React from 'react';
import '../Styles/Nav.css'
import { IoMdTimer } from "react-icons/io";

function Nav(){
    return <div>
        <nav>
            <a href="#"><IoMdTimer color='whitesmoke'/> HORA CERTA<IoMdTimer color='whitesmoke' /></a>
        </nav>
        <hr />
    </div>
}

export default Nav;