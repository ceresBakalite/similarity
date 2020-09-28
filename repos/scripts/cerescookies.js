function setCookie(name, value, options = {}) {

    // Example of use:
    //setCookie('user', 'John', {secure: true, 'max-age': 3600});

    options = {
        path: '/',
        // add other defaults here if necessary
        //...options
    };

    if (options.expires instanceof Date) { options.expires = options.expires.toUTCString(); }

    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    for (let optionKey in options)
    {
        updatedCookie += "; " + optionKey;
        let optionValue = options[optionKey];
        if (optionValue !== true)
        {
            updatedCookie += "=" + optionValue;
        }

    }

    document.cookie = updatedCookie;
}

function getCookie(cn)
{
    if (cn)
    {
        let cp = cn + '=';
        let dc = decodeURIComponent(document.cookie);
        let ca = dc.split(';');

        console.log(dc);

        for(let i = 0; i < ca.length; i++)
        {
            let chr = ca[i];

            while (chr.charAt(0) == String.fromCharCode(32)) chr = chr.substring(1);

            if (chr) return chr.substring(cn.length, c.length);
        }

    }

    return null;
}
