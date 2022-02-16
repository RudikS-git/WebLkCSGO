import React from 'react'
import { connect } from 'react-redux'
import { modalHide } from '../../reducers/modal'
import './ModalContainer.css'

class ModalContainer extends React.Component {

    closeModal = () => {
        const { CloseModal, onClose = () => {} } = this.props
        CloseModal();
        onClose();
    }

    render() {
        const { modal: { modalType, modalProps } } = this.props;
        const SpecificModal = modalType;

        return(
        <>
            {modalType && (
            <div className="modalContainer">
                <div className="modalContainer__overlay" 
                 onClick={this.closeModal} />

                 <div className="modalContainer__content">
                    
                    {modalProps && modalProps.hasClose && (
                        <button className="modalContainer__close"
                                onClick={this.closeModal}>
                        </button>
                    )} 

                    <div className="modalContainer__inner">
                        <SpecificModal {...this.props} />
                    </div>
                    
                 </div>
            </div>)}
            
        </>)
    }
}   

const mapStateToProps = store => {
  return {
    modal: ({...store.modal}),
  }
}

const mapDispatchToProps = dispatch => {
  return {
    CloseModal: () => dispatch(modalHide()),
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ModalContainer)