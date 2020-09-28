function setCookie(cn, cv, ex = 0)
{
    if (cn && cv)
    {
        let dt = new Date();
        dt.setTime(dt.getTime() + (ex * 24 * 60 * 60 * 1000));
        let expires = 'expires=' + dt.toUTCString();
        let domain = 'domain=ceresbakalite.github.io/similarity';

console.log(cn + '=' + cv + ';' + domain + ';' + expires + ';path=/');

        document.cookie = encodeURIComponent(cn) + '=' + encodeURIComponent(cv) + ';' + encodeURIComponent(domain) + ';' + expires + ';path=/';
    }

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
