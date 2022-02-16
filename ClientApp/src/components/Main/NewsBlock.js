import React, { Component } from 'react';

import './NewsBlock.css';

import AwesomeSlider from 'react-awesome-slider';
import withAutoplay from 'react-awesome-slider/dist/autoplay';
import 'react-awesome-slider/dist/styles.css';

const AutoplaySlider = withAutoplay(AwesomeSlider);

export class NewsBlock extends Component 
{
  displayName = NewsBlock.name

    constructor(props) 
    {
        super(props);
        //this.state = { servers: [], loading: true, players: null, slots: null };

       /* fetch("ServerMonitoring/GetServersInfo")
        .then(response => response.json())
        .then(data => {
            this.setState({ servers: data.servers, players: data.players, slots: data.slots, loading: false });
        });*/
    }

    render()
    { 
        var settings = {
            dots: true,
            infinite: true,
            speed: 500,
            slidesToShow: 1,
            slidesToScroll: 1
          };

        return (    
            <>

                <div className="news-block">

                  <AutoplaySlider
   
                    play={true}
                    cancelOnInteraction={false} // should stop playing on user interaction
                    interval={6000}
                  >
                      <div data-src="\images\aaddvv1.jpg" />
                      <div data-src="\images\aaddvv2.jpg" />
                  </AutoplaySlider>
             </div>       

            </>
        );
    }
}
