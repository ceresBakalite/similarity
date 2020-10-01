var cookies = {};
(function() {

    this.get = function (name)
    {
      let match = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
      return match ? decodeURIComponent(match[1]) : undefined;
    }

    this.set = function (name, value, options = {})
    {
        options = {
            path: '/',
        };

        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        let cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value);

        for (let item in options)
        {
            cookie += '; ' + item;
            //let option = options[item];
            //if (option) cookie += '=' + option;
            cookie = (options[item]) ? cookie += '=' + options[item] : cookie;
        }

        document.cookie = cookie;
    }

}).apply(cookies);
