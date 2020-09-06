window.customElements.define('include-directive', class extends HTMLElement
{
    async connectedCallback()
    {
        let src = this.getAttribute('src');
        this.innerHTML = await (await fetch(src)).text();
    }

});

function getQueryString()
{
    const urlParams = new URLSearchParams(window.location.search);
    const mdd = urlParams.get('mdd')

    if (mdd != null) selectMarkdownDocument(mdd);
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

        switch (md)
        {
          case 'index':
            setTimeout(function(){ asyncPullRequest('index-md'); }, ms);
            break;

          case 'shell':
            setTimeout(function(){ asyncPullRequest('shell-md'); }, ms);
            break;

          case 'repos':
            setTimeout(function(){ asyncPullRequest('repos-md'); }, ms);
            break;

          case 'slide':
            setTimeout(function(){ initialiseSlideViewer(); }, ms);

            break;

          default:
            setTimeout(function(){ asyncPullRequest('index-md'); }, ms);
            break;

        }

    }

    function isValidSource(md)
    {
        if (parent.document.getElementById('primary-container')) return true;

        window.location.href = 'https://ceresbakalite.github.io/similarity/?mdd=' + md;

        return false;
    }

    function invokeScrollEventListener()
    {
        window.onscroll = function(){ adjustHeaderDisplay(); };
    }

    function displayFooter()
    {
      document.getElementById('site-footer-display').style.display = 'block';
      document.getElementById('footer-content').style.display = 'block';
    }

    function initialiseSlideViewer(target)
    {
        displayFooter();
        ceres.slideViewer();
    }

    function asyncPullRequest(target)
    {
        displayFooter();
        setTimeout(refreshMarkdown(target), 3000);
    }

    function refreshMarkdown(target)
    {
        let el = (document.getElementById(target)) ? document.getElementById(target) : document.getElementsByTagName('zero-md')[0];
        if (el != null) el.setAttribute('src', el.getAttribute('src') + '?' + getRandomInteger());
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
    let pin = window.top.document.getElementById('pin-default').getAttribute('state');
    let trigger = 25;

    if (pin == 'disabled')
    {
        var el = parent.document.getElementById('site-header-display');

        if (window.scrollY < trigger || el.style.display == null)
        {
            if (el.style.display != 'block') setTimeout(function(){ setStyleDisplay('block'); }, 250);

        } else {

            if (el.style.display != 'none') setTimeout(function(){ setStyleDisplay('none'); }, 250);

        }

    }

    function setStyleDisplay(attribute)
    {
      el.style.display = attribute;
    }

}

function resetPinState()
{
    let el = document.getElementById('pin-default');

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
    if (cn != null && cv != null)
    {
        let dt = new Date();
        dt.setTime(dt.getTime() + (ex * 24 * 60 * 60 * 1000));
        let expires = "expires=" + dt.toUTCString();

        document.cookie = cn + "=" + cv + ";" + expires + ";path=/";
    }

}

function getCookie(cn)
{
    if (cn != null)
    {
        let cp = cn + "=";
        let dc = decodeURIComponent(document.cookie);
        let ca = dc.split(';');

        for(var i = 0; i < ca.length; i++)
        {
            let chr = ca[i];

            while (chr.charAt(0) == String.fromCharCode(32)) chr = chr.substring(1);

            if (chr != null) return chr.substring(cn.length, c.length);
        }

    }

    return null;
}
