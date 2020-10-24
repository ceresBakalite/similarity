/**
 * @license
 * cerescookies v1.0.0
 *
 * Minified using terser v5.3.5
 * Original file: ceresbakalite/similarity/repos/scripts/cerescookies.js
 *
 * ceresBakalite/similarity is licensed under the MIT License - http://opensource.org/licenses/MIT
 *
 * Copyright (c) 2020 Alexander Munro
*/
export { cookies }

var cookies = {};
(function() {

    this.get = function (name)
    {
        let match = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return match ? decodeURIComponent(match[1]) : undefined;
    }

    this.set = function (name, value, options = {})
    {
        let cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        if (!options.path) options.path = '/';
        if (!options.samesite) options.samesite = 'Lax; Secure';
        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        for (let item in options)
        {
            cookie += '; ' + item + '=' + ((typeof options[item] != null) ? options[item] : null);
        }

        document.cookie = cookie;
    }

}).call(cookies);
