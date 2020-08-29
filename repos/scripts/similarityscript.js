customElements.define('include-directive', class extends HTMLElement
{
  async connectedCallback()
  {
    let src = this.getAttribute('src');
    this.innerHTML = await (await fetch(src)).text();
  }

});

function getMarkdown(ms, location)
{
  switch (location)
  {
    case 'index':
      refreshMarkdown('index-md', 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000));
      break;

    case 'shell':
      refreshMarkdown('shell-md', 'https://ceresbakalite.github.io/similarity/shell/README.md?' + getRandomInteger(10000,1000000));
      break;

    case 'repos':
      refreshMarkdown('repos-md', 'https://ceresbakalite.github.io/similarity/README.md?' + getRandomInteger(10000,1000000));
      break;

    default:
      refreshMarkdown('index-md', 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000));
      break;

  }

  setInterval(waitForMarkdown, ms);
}

function refreshMarkdown(id, target)
{
  WebComponents.waitFor(() =>
  {
      let el = document.getElementById(id);
      el.setAttribute('src', target);
  });

}

function waitForMarkdown()
{
  WebComponents.waitFor(() =>
  {
      document.getElementById('site-footer-display').style.display = 'block';
  });

}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
