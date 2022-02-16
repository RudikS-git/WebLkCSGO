import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Modal from '@mui/material/Modal';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 800,
  backgroundColor: '#1a1919',
  border: '2px solid #000',
  boxShadow: 24,
  p: 4,
};

const PlayersModal = ({players, open, setOpen}) => {
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
       <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Игроки онлайн
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
          <TableContainer component={Paper}>
            <Table sx={{ color: 'white !important', backgroundColor: '#1a1919', minWidth: 650 }} size="small" aria-label="a dense table">
              <TableHead>
                <TableRow>
                  <TableCell sx={{color: 'white'}}>Ник</TableCell>
                  <TableCell sx={{color: 'white'}} align="right">Счет</TableCell>
                  <TableCell sx={{color: 'white'}} align="right">Время игры</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {players?.map((row) => (
                  <TableRow
                    key={row.name}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell sx={{color: 'white'}} component="th" scope="row">
                      {row.name}
                    </TableCell>
                    <TableCell sx={{color: 'white'}} align="right">{row.score}</TableCell>
                    <TableCell sx={{color: 'white'}} align="right">{row.duration.split('.')[0]}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          </Typography>
        </Box>
      </Modal>
    </div>
  );
}

export default PlayersModal;