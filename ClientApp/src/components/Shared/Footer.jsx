import React, { Component } from 'react';
import { STEAM_GROUP_LINK, VK_GROUP_LINK } from '../../CONST';
import './Footer.css';

export class Footer extends Component {

    render() {
        return (
            <div className="footer__div">    
                {/* <div className="shre-button">
                    <span className="shre__span"><img src="images/share.png" className="shre__png_span"></img>Связь</span>
                   
                </div>   */}

                <div className="footer__links">
                    <a href={VK_GROUP_LINK} className="shre__link" target="_blank"><img src="images/vcontact.png" className="shre__png"></img></a>
                    {/* <a href="https://discord.gg/76TJZfA" className="shre__link" target="_blank"><img src="images/Discord_icon.png"  className="shre__png"></img></a> */}
                    <a href={STEAM_GROUP_LINK} className="shre__link" target="_blank"><img src="images/icon-steam.png" className="shre__png"></img></a> 
                </div>
                                    

                <p className="footer__author">&copy;2020-2022. BinGo</p>
            </div>
        );
    }
}
