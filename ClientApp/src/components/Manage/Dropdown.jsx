import React, { useState, useRef } from 'react';

import './Dropdown.css';
import styled from 'styled-components';
import useIsActiveState from './useIsActiveState';

export const Dropdown = (props) => {
    const [isActive, setActive] = useState(false);

    const ref = useRef();
    
    useIsActiveState(ref, () => {
        if (isActive) {
            setActive(false);
        }
    });

    return <div className="dropdown">
                <button className="dropdown__btn" onClick={() => setActive(!isActive)}>
                    {
                        isActive?   
                            <i class="fas fa-chevron-up"></i> 
                            :
                            <i class="fas fa-chevron-down"></i>
                    }
                </button>

                <div className={isActive? "dropdown__content dropdown__content_active":"dropdown__content"}>
                    {
                        props.isHeader &&
                            <span className="dropdown__header">{props.headerText}</span>
                    }
                    
                    <div className="dropdown__list">
                        {
                            props.items && props.items.map(it =>
                                
                                <p ref={ref} className="dropdown__list_item">{it.content}</p>
                            )
                        }
                    </div>
                </div>
                
            </div>
}