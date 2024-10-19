if (typeof jQuery === 'undefined') {
    throw new Error('Requires jQuery library.');
}

(function ($) {
    'use strict';
    $.wrapAjax = function (options) {

        // Deafaults settings
        this.defaults = {
            ShowLoading: true,
            LoaderContainerId: "#preloader",
            ErrorCallBack: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest) {
                    const errorText = `${XMLHttpRequest?.status} - ${XMLHttpRequest?.responseText}`;
                    console.log(errorText);
                    $.toast({ type: "error", text: "Something went wrong. Please try again later.", heading: 'Error' });
                }
            }
        };

        //Merge the default and provided options
        var obj = $.extend({}, this.defaults, options);

        //Get ajax wrapper
        const get = function (url, data, showLoading, successCallback, errorCallback) {

            if (showLoading != null ? showLoading : obj.ShowLoading) {
                ShowLoader();
            }

            if (!url) {
                return 1; // Incorrect Parameters
            }


            if (!errorCallback) {
                errorCallback = obj.ErrorCallBack;
            }


            if (data != null && typeof (data) === "object") {
                data = $.param(data);
            }


            $.ajax({
                type: 'GET',
                url: url,
                data: data,
                //contentType: 'application/json; charset=utf-8',
                success: successCallback,
                error: errorCallback
            });

            return 0;
        }

        //post ajax wrapper
        const post = function (url, parameters, showLoading, successCallback, errorCallback) {
            if (showLoading != null ? showLoading : obj.ShowLoading) {
                ShowLoader();
            }

            if (!url) {
                return 1; // Incorrect Parameters
            }

            if (!errorCallback) {
                errorCallback = obj.ErrorCallBack;
            }

            //parameters = JSON.stringify(parameters);

            //if (typeof (parameters) === "object") {
            //    parameters = JSON.stringify(parameters);
            //}

            $.ajax({
                type: 'POST',
                url: url,
                data: parameters,
                //contentType: 'application/json;',
                success: successCallback,
                error: errorCallback
            });

            return 0;
        }


        function ShowLoader() {
            //console.log('on loader');
            let count = $(obj.LoaderContainerId).attr('data-count');
            if (count && parseInt(count) != NaN) {
                count++;
                $(obj.LoaderContainerId).attr('data-count', count);
            }
            $(obj.LoaderContainerId).show();
            // $(obj.LoaderContainerId).fadeIn("fast");
        };

        $(document).ready(function () {
            $(this).ajaxStop(function () {
                //console.log('stop loader');
                let count = $(obj.LoaderContainerId).attr('data-count');
                if (count && parseInt(count) != NaN && count > 0) {
                    count--;
                    $(obj.LoaderContainerId).attr('data-count', count);
                }
                if (count < 1) {
                    $(obj.LoaderContainerId).hide();
                }
            });

        });

        return {
            post: post,
            get: get
        };

    }


})(jQuery);