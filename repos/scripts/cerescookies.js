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
        if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

        var d = new Date();
        d.setTime(d.getTime() + (3*24*60*60*1000));
        options.expires = d.toUTCString();

        for (let item in options)
        {
            cookie += '; ' + item + '=' + ((typeof options[item] != null) ? options[item] : null);
        }

console.log(cookie);

        document.cookie = cookie;
    }

}).apply(cookies);
