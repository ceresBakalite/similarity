customElements.define('include-directive', class extends HTMLElement
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
        setTimeout(function(){ waitForMarkdown('index-md'); }, ms);
        break;

      case 'shell':
        setTimeout(function(){ waitForMarkdown('shell-md'); }, ms);
        break;

      case 'repos':
        setTimeout(function(){ waitForMarkdown('repos-md'); }, ms);
        break;

      default:
        setTimeout(function(){ waitForMarkdown('index-md'); }, ms);
        break;

    }

  }

  function isValidSource(md)
  {
      if (parent.document.getElementById('primary-container')) return true;

      window.location.href = 'https://ceresbakalite.github.io/similarity/?mdd=' + md;

      return false;
  }

  function waitForMarkdown(target)
  {
      document.getElementById('site-footer-display').style.display = 'block';
      document.getElementById('footer-content').style.display = 'block';

      refreshMarkdown(target);
  }

  function refreshMarkdown(target)
  {
    if (document.getElementById(target))
    {
      document.getElementById(target).setAttribute('src', document.getElementById(target).getAttribute('src') + '?' + getRandomInteger());

    } else if (document.getElementsByTagName('zero-md')[0]) {

      document.getElementsByTagName('zero-md')[0].setAttribute('src', document.getElementsByTagName('zero-md')[0].getAttribute('src') + '?' + getRandomInteger());
    }

  }

  function invokeScrollEventListener()
  {
    window.onscroll = function(){ adjustHeaderDisplay(); };

    function adjustHeaderDisplay()
    {
alert('start');
      let pin = parent.document.getElementById('pin-default');
      alert('pin');
      let state = (pin.state == null) ? 'disabled' : pin.state;
      alert('end pin start');
alert(pin.state);
alert('end pin state');
//      if (state === 'enabled')
//      {
        let el = parent.document.getElementById('site-header-display');

        if (window.scrollY < 350 || el.style.display == null)
        {
          if (el.style.display !== 'block') resetDisplay('block');

        } else {

          if (el.style.display !== 'none') resetDisplay('none');

        }

//      }

      function resetDisplay(attribute)
      {
        el.style.display = attribute;
      }

    }

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

function resetPinState()
{
  let el = document.getElementById('pin-default');
  let state = (el.state == null) ? 'disabled' : el.state;

  if (state === 'disabled')
  {
      el.src = "https://ceresbakalite.github.io/similarity/images/NAVPinIconEnabled.png";
      el.state = 'enabled';
  }
  else
  {
      el.src = "https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png";
      el.state = 'disabled';
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
