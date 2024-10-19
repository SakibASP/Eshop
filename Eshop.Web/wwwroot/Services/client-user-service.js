/*import pager from '../../pager-model.js';*/

if (typeof jQuery === 'undefined') {
    throw new Error('Requires jQuery library.');
}

let showLoader = true;

const clientUserService = (function ($) {
    'use strict';
    /* Constant */
    const ajaxWraper = $.wrapAjax();

    /* Function Area */


    /* Save User Service */
    const saveUserService = function (payload, successCallback, errorCallback) {
        const URL = '/ClientUserService/Save';
        return ajaxWraper.post(
            URL,
            payload,
            showLoader,
            successCallback,
            errorCallback
        );
    }


    /* Update User Service */
    const updateUserService = function (payload, successCallback, errorCallback) {
        const URL = '/ClientUserService/Update';

        return ajaxWraper.post(
            URL,
            payload,
            showLoader,
            successCallback,
            errorCallback
        );
    }


    /* get User Service */
    const getUserService = function (payload, successCallback, errorCallback) {
        const URL = '/ClientUserService/getSingleUserService';

        return ajaxWraper.post(
            URL,
            payload,
            showLoader,
            successCallback,
            errorCallback
        );
    }


    const deleteUserServiceDetails = function (payload, successCallback, errorCallback) {
        const URL = '/ClientUserService/deleteUserServiceDetails';

        return ajaxWraper.post(
            URL,
            payload,
            showLoader,
            successCallback,
            errorCallback
        );
    }

    //const deleteUserService = function (payload, successCallback, errorCallback) {
    //    const URL = '/ClientUserService/DeleteSingleUserService';

    //    return ajaxWraper.post(
    //        URL,
    //        payload,
    //        showLoader,
    //        successCallback,
    //        errorCallback
    //    );
    //}

    return {
        saveUserService: saveUserService,
        updateUserService: updateUserService,
        getUserService: getUserService,
        //deleteUserService: deleteUserService,
        deleteUserServiceDetails: deleteUserServiceDetails
    }
})(jQuery);

export default clientUserService;