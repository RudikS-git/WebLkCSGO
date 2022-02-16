import React from 'react';
import "./LoadingSpinner.css";

export const LoadingSpinner = () => (
    <><div className="showbox">
        <div className="loader rot">
            {/* <svg className="circular" viewBox="25 25 50 50">
                <circle className="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"/>
            </svg> */}
            <img className="loader__ker" src="images/kerambit.png" />
        </div>
    </div>
    </>)
