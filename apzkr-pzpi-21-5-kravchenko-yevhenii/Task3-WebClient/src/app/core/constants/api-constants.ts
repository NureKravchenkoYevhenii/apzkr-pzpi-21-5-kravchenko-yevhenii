import { Observable, OperatorFunction, tap } from "rxjs";
import { environment } from "../../../environments/environment.development";

const BASE_URL = environment.webApi;

const AUTH_URL = `${BASE_URL}auth`;
const LOGIN = 'login';
const REFRESH = 'refresh';

const ADMINISTRATION_URL = `${BASE_URL}administrations`;
const BACKUP_DATABASE = 'backup-database';
const UPDATE_PARKING_SETTINGS = 'update-parking-settings';
const GET_PARKING_SETTINGS = 'get-parking-settings';

const BOOKINGS_URL = `${BASE_URL}bookings`;
const MAKE_BOOKING = 'make-booking';
const CANCEL_BOOKING = 'cancel-booking';
const GET_USER_BOOKINGS = 'get-user-bookings';

const MEMBERSHIPS_URL = `${BASE_URL}memberships`;
const CREATE_MEMBERSHIP = 'create';
const DELETE_MEMBERSHIP = 'delete';
const GET_ALL_MEMBERSHIPS = '';
const GET_MEMBERSHIP = 'get';
const UPDATE_MEMBERSHIP = 'update';

const PARKING_PLACES_URL = `${BASE_URL}parking-places`;
const CREATE_PARKING_PLACE = 'create';
const DELETE_PARKING_PLACE = 'delete';
const GET_ALL_PARKING_PLACES = '';
const GET_PARKING_PLACE = 'get';
const UPDATE_PARKING_PLACE = 'update';
const GET_OCCUPANCY_STATISTICS = 'occupancy-statistics';

const PAYMENTS_URL = `${BASE_URL}payments`;
const GET_PAYMENT_STATISTICS = 'get-statistics';

const TARIFS_URL = `${BASE_URL}tarifs`;
const CREATE_TARIF = 'create';
const DELETE_TARIF = 'delete';
const GET_ALL_TARIFS = '';
const GET_TARIF = 'get';
const UPDATE_TARIF = 'update';

const USERS_URL = `${BASE_URL}users`;
const REGISTER = 'register';
//const UPDATE_PROFILE = 'update';
const GET_ALL_USERS = 'get-all';
const GET_USER = 'get';
const SET_USER_ROLE = 'set-user-role';
const BLOCK_MEMBERSHIP = 'block-membership';
const UPLOAD_USER_DATA = 'upload-user-data';

const PARKING_SESSIONS_URL = `${BASE_URL}parking-sessions`;
const GET_PARKING_STATISTICS = 'get-parking-statistics';

export const API = {
    auth: {
        login: `${AUTH_URL}/${LOGIN}`,
        refresh: `${AUTH_URL}/${REFRESH}`,
    },
    administration: {
        backupDatabase: `${ADMINISTRATION_URL}/${BACKUP_DATABASE}`,
        updateParkingSettings: `${ADMINISTRATION_URL}/${UPDATE_PARKING_SETTINGS}`,
        getParkingSettings: `${ADMINISTRATION_URL}/${GET_PARKING_SETTINGS}`
    },
    bookings: {
        makeBooking: `${BOOKINGS_URL}/${MAKE_BOOKING}`,
        cancelBooking: `${BOOKINGS_URL}/${CANCEL_BOOKING}`,
        getUserBookings: `${BOOKINGS_URL}/${GET_USER_BOOKINGS}`
    },
    memberships: {
        create: `${MEMBERSHIPS_URL}/${CREATE_MEMBERSHIP}`,
        delete: `${MEMBERSHIPS_URL}/${DELETE_MEMBERSHIP}`,
        getAll: `${MEMBERSHIPS_URL}/${GET_ALL_MEMBERSHIPS}`,
        get: `${MEMBERSHIPS_URL}/${GET_MEMBERSHIP}`,
        update: `${MEMBERSHIPS_URL}/${UPDATE_MEMBERSHIP}`
    },
    parkingPlaces: {
        create: `${PARKING_PLACES_URL}/${CREATE_PARKING_PLACE}`,
        delete: `${PARKING_PLACES_URL}/${DELETE_PARKING_PLACE}`,
        getAll: `${PARKING_PLACES_URL}/${GET_ALL_PARKING_PLACES}`,
        get: `${PARKING_PLACES_URL}/${GET_PARKING_PLACE}`,
        update: `${PARKING_PLACES_URL}/${UPDATE_PARKING_PLACE}`,
        getOccupancyStatistics: `${PARKING_PLACES_URL}/${GET_OCCUPANCY_STATISTICS}`
    },
    payments: {
        getPaymentStatistics: `${PAYMENTS_URL}/${GET_PAYMENT_STATISTICS}`
    },
    tarifs: {
        create: `${TARIFS_URL}/${CREATE_TARIF}`,
        delete: `${TARIFS_URL}/${DELETE_TARIF}`,
        getAll: `${TARIFS_URL}/${GET_ALL_TARIFS}`,
        get: `${TARIFS_URL}/${GET_TARIF}`,
        update: `${TARIFS_URL}/${UPDATE_TARIF}`
    },
    user: {
        register: `${USERS_URL}/${REGISTER}`,
        getAll: `${USERS_URL}/${GET_ALL_USERS}`,
        get: `${USERS_URL}/${GET_USER}`,
        setUserRole: `${USERS_URL}/${SET_USER_ROLE}`,
        blockMembership: `${USERS_URL}/${BLOCK_MEMBERSHIP}`,
        uploadUserData: `${USERS_URL}/${UPLOAD_USER_DATA}`
    },
    parkingSessions: {
        getParkingStatistics: `${PARKING_SESSIONS_URL}/${GET_PARKING_STATISTICS}`
    }
};

export function extractData<T>(): OperatorFunction<any, any> {
    return (source: Observable<T>) =>
    source.pipe(
        tap(data => data)
    );
}