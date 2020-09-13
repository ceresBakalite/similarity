window.customElements.define('include-directive', class extends HTMLElement
{
    async connectedCallback()
    {
        let src = this.getAttribute('src');
        this.innerHTML = await (await fetch(src)).text();
    }

});

/*
function getQueryString()
{
    const urlParams = new URLSearchParams(window.location.search);
    const mdd = urlParams.get('mdd')

    if (mdd) selectMarkdownDocument(mdd);
}
*/

function getQueryString()
{
    const urlParams = new URLSearchParams(window.location.search);
    const md = urlParams.get('mdd')

    if (md) getMarkdownDocument(md);
}

function getMarkdownDocument(md)
{
    document.getElementById('frame-container').setAttribute('src', getMarkdownLocation());
    document.getElementById(md).blur();

    function getMarkdownLocation()
    {
       const lookup = {
           'index': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html',
           'shell': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html',
           'slide': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html',
           'repos': 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html'
       };

       return lookup[md] || 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html';
    }

}

function onloadPrimary()
{
    getQueryString();
}

function onloadFrame(ms, md)
{
    if (isValidSource(md))
    {
        invokeScrollEventListener();

        const initialise = {
            'index': function() { setTimeout(function() { asyncPullRequest('index-md'); }, ms); },
            'shell': function() { setTimeout(function() { asyncPullRequest('shell-md'); }, ms); },
            'slide': function() { setTimeout(function() { initialiseSlideViewer(); }, ms); },
            'repos': function() { setTimeout(function() { asyncPullRequest('repos-md'); }, ms); }
        };

        initialise[md]() || initialise['index']();
    }

    function isValidSource(md)
    {
        if (parent.document.getElementById('primary-container')) return true;

        window.location.href = 'https://ceresbakalite.github.io/similarity/?mdd=' + md;

        return false;
    }

    function invokeScrollEventListener()
    {
        window.onscroll = function() { adjustHeaderDisplay(); };
    }

    function displayFooter()
    {
      document.getElementById('site-footer-display').style.display = 'block';
      document.getElementById('footer-content').style.display = 'block';
    }

    function initialiseSlideViewer()
    {
        displayFooter();
    }

    function asyncPullRequest(target)
    {
        displayFooter();
        setTimeout(function() { refreshMarkdown(target); }, 5000);
    }

    function refreshMarkdown(target)
    {
        let el = (document.getElementById(target)) ? document.getElementById(target) : document.getElementsByTagName('zero-md')[0];
        if (el) el.setAttribute('src', el.getAttribute('src') + '?' + getRandomInteger());
    }

}

function selectMarkdownDocument(md)
{
    switch (md)
    {
        case 'index':
          getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
          break;

        case 'shell':
          getMarkdownDocument('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
          break;

        case 'repos':
          getMarkdownDocument('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');
          break;

        case 'slide':
          getMarkdownDocument('slide', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html');
          break;

        default:
          getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
          break;

    }

    function getMarkdownDocument(id, target)
    {
        document.getElementById('frame-container').setAttribute('src', target);
        document.getElementById(id).blur();
    }

}

function adjustHeaderDisplay()
{
    let el = parent.document.getElementById('site-header-display');
    let pin = window.top.document.getElementById('pin-navbar').getAttribute('state');
    let trigger = 25;

    if (pin == 'disabled')
    {
        if (el.style.display && window.scrollY > trigger)
        {
            if (el.style.display != 'none') setTimeout(function(){ setStyleDisplay('none'); }, 250);

        } else {

            if (el.style.display != 'block') setTimeout(function(){ setStyleDisplay('block'); }, 250);

        }

    }

    function setStyleDisplay(attribute)
    {
      el.style.display = attribute;
    }

}

function resetPinState()
{
    let el = document.getElementById('pin-navbar');

    if (el.getAttribute('state') == 'enabled')
    {
        el.src = "https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png";
        el.setAttribute('state', 'disabled');
        adjustHeaderDisplay();

    } else {

        el.src = "https://ceresbakalite.github.io/similarity/images/NAVPinIconEnabled.png";
        el.setAttribute('state', 'enabled');

    }

}

function getRandomInteger(min = 10000, max = 1000000)
{
    return Math.floor(Math.random() * (max - min) ) + min;
}

function setCookie(cn, cv, ex = 0)
{
    if (cn && cv)
    {
        let dt = new Date();
        dt.setTime(dt.getTime() + (ex * 24 * 60 * 60 * 1000));
        let expires = "expires=" + dt.toUTCString();

        document.cookie = cn + "=" + cv + ";" + expires + ";path=/";
    }

}

function getCookie(cn)
{
    if (cn)
    {
        let cp = cn + "=";
        let dc = decodeURIComponent(document.cookie);
        let ca = dc.split(';');

        for(let i = 0; i < ca.length; i++)
        {
            let chr = ca[i];

            while (chr.charAt(0) == String.fromCharCode(32)) chr = chr.substring(1);

            if (chr) return chr.substring(cn.length, c.length);
        }

    }

    return null;
}
