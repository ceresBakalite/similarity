class Cookies {

    constructor()
    {
        this.name;
        this.value;
        this.options = {};
    }

    set()
    {
        this.options = {
            path: '/',
        };

        if (this.options.expires instanceof Date) { this.options.expires = this.options.expires.toUTCString(); }

        let updatedCookie = encodeURIComponent(this.name) + '=' + encodeURIComponent(this.value);

        for (let optionKey in this.options)
        {
            updatedCookie += '; ' + optionKey;
            let optionValue = this.options[optionKey];
            if (optionValue !== true)
            {
                updatedCookie += '=' + optionValue;
            }

        }

        document.cookie = updatedCookie;
    }

    get()
    {
        let matches = document.cookie.match(new RegExp("(?:^|; )" + this.name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    }

    function xxxsetCookie(name, value, options = {})
    {
        //setCookie('user', 'John', {secure: true, 'max-age': 3600});

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

    function xxxgetCookie(name)
    {
        let matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    }

}
