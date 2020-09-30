var similarity = {};
(function(ceres)
{
    window.customElements.define('include-directive', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    ceres.onloadPrimary = function() { onloadPrimary(); }; // public method reference
    ceres.getMarkupDocument = function(id, el) { getMarkupDocument(id, el); };  // public method reference
    ceres.resetPinState = function(el) { resetPinState(el); };  // public method reference

    class Component
    {
        constructor()
        {
            this.attribute = function() { return attribute; }
        }

    }

    let resource = new Component();
    let location = new Map();
    let pinimage = new Map();

    setResourcePrecursors();

    function getQueryString()
    {
        const urlParams = new URLSearchParams(window.location.search);
        const markupId = urlParams.get('mu')

        if (markupId) getMarkupDocument(markupId);
    }

    function getMarkupDocument(markupId, buttonElement)
    {
        if (resource.attribute.markupId != markupId)
        {
            resource.attribute.markupId = markupId;
            resource.attribute.markupUrl = (location.has(resource.attribute.markupId)) ? location.get(resource.attribute.markupId) : location.get('index');

            document.getElementById('frame-container').setAttribute('src', resource.attribute.markupUrl);
        }

        if (buttonElement) buttonElement.blur();

        cookies.set('mi', resource.attribute.markupId, { 'max-age': 3600 });
    }

    function establishHeaderAttributes()
    {
        if (cookies.get('hd') == 'none') document.getElementById('site-header-display').style.display = 'none';
        if (cookies.get('pn') == 'enabled') setPinState(document.getElementById('pin-navbar'), 'enabled');

        resource.attribute.markupId = cookies.get('mi');
    }

    function onloadPrimary()
    {
        establishHeaderAttributes();
        getQueryString();
    }

    function setResourcePrecursors()
    {
        pinimage.set('enabled', 'https://ceresbakalite.github.io/similarity/images/NAVPinIconEnabled.png');
        pinimage.set('disabled', 'https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png');

        location.set('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
        location.set('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
        location.set('slide', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html');
        location.set('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');

        resource.attribute.markupId = 'index';
        resource.attribute.markupUrl = location.get(resource.attribute.markupId);
    }

    function resetPinState(el)
    {
        if (el.getAttribute('state') == 'enabled')
        {
            let attribute = 'disabled';

            setPinState(el, attribute);
            setDisplayState(attribute);

        } else {

            setPinState(el, 'enabled');
        }

    }

    function setDisplayState(attribute)
    {
        let header = document.getElementById('site-header-display');
        if (header.style.display != 'block') setTimeout(function() { header.style.display = 'block'; }, 250);
        cookies.set('hd', attribute, { 'max-age': 3600 });
    }

    function setPinState(el, attribute)
    {
        el.src = pinimage.get(attribute);
        el.setAttribute('state', attribute);
        cookies.set('pn', attribute, { 'max-age': 3600 });
    }

})(similarity);
