let cookies = {};
(function(cookie)
{

    cookie.getCookie = function(name) { getCookie(name); }; // public method reference
    cookie.setCookie = function(name, value) { setCookie(name, value, options = {}); };  // public method reference

    function setCookie(name, value, options = {})
    {
        options = {
            path: '/',
        };

        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        let updatedCookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        for (let optionKey in options)
        {
            updatedCookie += '; ' + optionKey;
            let optionValue = options[optionKey];
            if (optionValue !== true)
            {
                updatedCookie += '=' + optionValue;
            }

        }

        document.cookie = updatedCookie;
    }

    function getCookie(name)
    {
        let matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    }

})(cookies);
