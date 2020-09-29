let similarity = {};
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

        window.parent.setCookie('mi', resource.attribute.markupId, { 'max-age': 3600 });
    }

    function establishHeaderAttributes()
    {
        if (window.parent.getCookie('pn') === 'enabled')
        {
            document.getElementById('site-header-display').style.display = window.parent.getCookie('hd');
            setPinState(document.getElementById('pin-navbar'), 'enabled');
        }

        resource.attribute.markupId = window.parent.getCookie('mi');
    }

    function onloadPrimary()
    {
        establishHeaderAttributes();
        getQueryString();
    }

    function setResourcePrecursors()
    {
        location.set('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
        location.set('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
        location.set('slide', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncSlide.html');
        location.set('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');

        resource.attribute.markupId = 'index';
        resource.attribute.markupUrl = location.get(resource.attribute.markupId);
    }

    function resetPinState(el)
    {
        if (el.getAttribute('state') === 'enabled')
        {
            setPinState(el, 'disabled');

        } else {

            setPinState(el, 'enabled');
        }

        if (window.parent.getCookie('hd') != 'none' )
        {
            let header = document.getElementById('site-header-display');
            if (header.style.display != 'block') setTimeout(function() { header.style.display = 'block'; }, 250);
        }

    }

    function setPinState(el, attribute)
    {
        el.src = (attribute === 'enabled') ? 'https://ceresbakalite.github.io/similarity/images/NAVPinIconEnabled.png' : 'https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png';
        el.setAttribute('state', attribute);
        window.parent.setCookie('pn', attribute, { 'max-age': 3600 });
    }

})(similarity);
