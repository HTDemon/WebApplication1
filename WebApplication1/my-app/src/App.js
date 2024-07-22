import './App.css';

import * as React from 'react';
import { useState, useEffect } from 'react';
import { styled } from '@mui/material/styles';
import PropTypes from 'prop-types';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import CssBaseline from '@mui/material/CssBaseline';
import useScrollTrigger from '@mui/material/useScrollTrigger';
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import DescriptionIcon from '@mui/icons-material/Description';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Cancel';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell, { tableCellClasses } from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import CloseIcon from '@mui/icons-material/Close';
import Slide from '@mui/material/Slide';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import NoteAddIcon from '@mui/icons-material/NoteAdd';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';


const StyledTableCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        backgroundColor: theme.palette.primary.main,
        color: theme.palette.common.white,
        fontSize: 20,
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 24,
    },
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
        backgroundColor: theme.palette.action.hover,
    },
    // hide last border
    '&:last-child td, &:last-child th': {
        border: 0,
    },
}));

const Transition = React.forwardRef(function Transition(props, ref) {
    return <Slide direction="up" ref={ref} {...props} />;
});

function GetOrder(props) {
    const initAddOrder = {
        patientId: props.pid,
        message: ""
    }
    const initEditOrder = {
        id: props.orderId,
        message: ""
    }
    const [order, setOrder] = React.useState([]);
    const [editMode, setEditMode] = React.useState(false);
    const [addOrder, setAddOrder] = React.useState(initAddOrder);
    const [editOrder, setEditOrder] = React.useState(initEditOrder);

    const handleEnterEditMode = () => {
        setEditMode(true);
        setEditOrder(order);
    };

    const handleExitEditMode = () => {
        setEditMode(false);
    };

    const handleAddOrder = async (event) => {
        event.preventDefault();
        console.log(addOrder)
        try {

            const response = await fetch(`${process.env.REACT_APP_API_URL}${process.env.REACT_APP_API_ADD_ORDER_URL}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(addOrder),
            });   

            if (response.ok) {
                console.log('Form submitted successfully!');
                setAddOrder(initAddOrder);
                window.location.reload();
                //const json = await response.json();
                //console.log("json ", json);
            } else {
                console.error('Form submission failed!');
            }

        } catch (error) {
            console.log(error.message);
        } finally {
            
        }
    }

    const handleEditOrder = async (event) => {
        event.preventDefault();
        console.log(editOrder)
        try {

            const response = await fetch(`${process.env.REACT_APP_API_URL}${process.env.REACT_APP_API_UPDATE_ORDER_URL}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(editOrder),
            });

            if (response.ok) {
                console.log('Form submitted successfully!');
                handleExitEditMode();
                window.location.reload();
            } else {
                console.error('Form submission failed!');
            }

        } catch (error) {
            console.log(error.message);
        } finally {

        }
    }

    useEffect(() => {
        fetch(`${process.env.REACT_APP_API_URL}${process.env.REACT_APP_API_GET_ORDER_URL}${props.orderId}`)
            .then(response => response.json())
            .then(json => setOrder(json)
            ).catch(err => console.log("Fetching patient failed"));
    }, [])

    //if (order.length === 0) {
    //    setOrder([{
    //        id: 0,
    //        message: "查無醫囑"
    //    }]);
    //}

    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 700 }} aria-label="customized table" padding="normal">
                <TableHead>
                    <TableRow>
                        <TableCell component="th" scope="row" colSpan={3}><TextField label="新增醫囑" fullWidth id="fullWidth" onChange={(e) => {
                            setAddOrder({ ...addOrder, message: e.target.value });
                        }} /></TableCell>
                        <TableCell align="right">
                            <Button color="secondary" size="large" variant="contained" startIcon={<NoteAddIcon />} onClick={handleAddOrder}>
                                新增醫囑
                            </Button>
                        </TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {order.map((o) => (
                        <StyledTableRow key={o.id}>
                            {!editMode && (
                                <>
                                    <StyledTableCell component="th" scope="row" colSpan={3}>
                                        {o.message}
                                    </StyledTableCell>
                                    <TableCell align="right" >
                                        <Tooltip title="編輯">
                                            <IconButton onClick={handleEnterEditMode}>
                                                <EditIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </>)}
                            {editMode && (
                                <>
                                    <StyledTableCell component="th" scope="row" colSpan={3}>
                                        <TextField fullWidth id="fullWidth" defaultValue={o.message} onChange={(e) => {
                                            setEditOrder({ id: o.id, message: e.target.value });
                                        }} />
                                    </StyledTableCell>
                                    <TableCell align="right" >
                                        <Tooltip title="儲存" onClick={handleEditOrder}>
                                            <IconButton>
                                                <SaveIcon />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="取消">
                                            <IconButton onClick={handleExitEditMode}>
                                                <CancelIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </>)}
                        </StyledTableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}

function PatientTable() {

    const [patients, setPatients] = React.useState([]);
    const [open, setOpen] = React.useState(false);
    const [info, setInfo] = React.useState([]);

    const handleClickOpen = (value) => () => {
        setInfo(value)
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    useEffect(() => {
        fetch(`${process.env.REACT_APP_API_URL}${process.env.REACT_APP_API_GET_PATIENT_URL}`)
            .then(response => response.json())
            .then(json => setPatients(json)
            ).catch(err => console.log("Fetching patient failed"));
    }, [])
    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 700 }} aria-label="customized table">
                <TableHead>
                    <TableRow>
                        <StyledTableCell>住民</StyledTableCell>
                        <StyledTableCell align="right">醫囑</StyledTableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {patients.map((p) => (
                        <StyledTableRow key={p.id}>
                            <StyledTableCell component="th" scope="row">
                                {p.name}
                            </StyledTableCell>
                            < TableCell align="right" >
                                <Tooltip title="醫囑">
                                    <IconButton onClick={handleClickOpen(p)}>
                                        <DescriptionIcon />
                                    </IconButton>
                                </Tooltip>
                                <React.Fragment>
                                    <Dialog
                                        fullScreen
                                        open={open}
                                        onClose={handleClose}
                                        TransitionComponent={Transition}
                                    >
                                        <AppBar sx={{ position: 'relative' }}>
                                            <Toolbar>
                                                <IconButton
                                                    edge="start"
                                                    color="inherit"
                                                    onClick={handleClose}
                                                    aria-label="close"
                                                >
                                                    <CloseIcon />
                                                </IconButton>
                                                <Typography sx={{ ml: 2, flex: 1 }} variant="h6" component="div">
                                                    住民 {info.name}的醫囑
                                                </Typography>
                                            </Toolbar>
                                        </AppBar>
                                        <GetOrder pid={info.id} orderId={info.orderId} />
                                    </Dialog>
                                </React.Fragment>
                            </TableCell >
                        </StyledTableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}

function ElevationScroll(props) {
    const { children, window } = props;
    // Note that you normally won't need to set the window ref as useScrollTrigger
    // will default to window.
    // This is only being set here because the demo is in an iframe.
    const trigger = useScrollTrigger({
        disableHysteresis: true,
        threshold: 0,
        target: window ? window() : undefined,
    });

    return React.cloneElement(children, {
        elevation: trigger ? 4 : 0,
    });
}

ElevationScroll.propTypes = {
    children: PropTypes.element.isRequired,
    /**
     * Injected by the documentation to work in an iframe.
     * You won't need it on your project.
     */
    window: PropTypes.func,
};

function ElevateAppBar(props) {
    return (
        <React.Fragment>
            <CssBaseline />
            <ElevationScroll {...props}>
                <AppBar>
                    <Toolbar>
                        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                            {process.env.REACT_APP_SITE_TITLE}
                        </Typography>
                        <Link href="/swagger/index.html" className="btn btn-primary-" color="inherit">Swagger UI</Link>
                    </Toolbar>
                </AppBar>
            </ElevationScroll>
            <Toolbar />
            <Container>
                <Box sx={{ my: 2 }}>
                    <PatientTable />
                </Box>
            </Container>
        </React.Fragment>
    );
}

function App() {
    return (
        <>
            <ElevateAppBar />
        </>
    );
}

export default App;
