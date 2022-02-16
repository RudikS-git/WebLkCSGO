import { credentials, domain } from '../configureFetch';
import { fatFetch } from '../utils/ajaxHelper';

export const GetTickets = (page) => {

  if(!page)
    page = 0;
    
  return fatFetch(`/api/admin/tickets/get?page=${page}&offset=50`, true)
}

export const GetTicket = (id) => {

  return fatFetch(`/api/admin/tickets/getbyid?id=${id}`, true)
}

export const GetTicketsByUser = (accusedUserStatId, ticketId) => {

    return fatFetch(`/api/admin/tickets/getticketsbyuser?accusedUserStatId=${accusedUserStatId}&ticketId=${ticketId}`, true)
}

export const SetTicketState = (checkingUserId, ticketId, state, answer) => {

  const ticket = { checkingUserId, ticketId, state, answer };

  return fatFetch(`/api/admin/tickets/setticketstate`, true, "POST", { 'Content-Type': 'application/json' }, JSON.stringify(ticket))
}