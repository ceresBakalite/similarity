export { cookies }

var cookies = {};
(function() {

    window.getCookie = function(str) { this.get(str); }; // public method reference
    window.setCookie = function(name, value, options) { this.set(name, value, options); };  // public method reference

    this.get = function (name)
    {
        let match = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return match ? decodeURIComponent(match[1]) : undefined;
    }

    this.set = function (name, value, options = {})
    {
        let cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        if (!options.path) options.path = '/';
        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        for (let item in options)
        {
            cookie += '; ' + item + '=' + ((typeof options[item] != null) ? options[item] : null);
        }

        document.cookie = cookie;
    }

}).apply(cookies);
