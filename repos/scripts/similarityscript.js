customElements.define('include-directive', class extends HTMLElement
{
  async connectedCallback()
  {
    let src = this.getAttribute('src');
    this.innerHTML = await (await fetch(src)).text();
  }

});

function getMarkdown(ms, target)
{
  let id = 'index-md';
  let target = 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000);

  switch (target)
  {
    case 'index':
      break;

    case 'shell':
      id = 'shell-md';
      target = 'https://ceresbakalite.github.io/similarity/shell/README.md?' + getRandomInteger(10000,1000000);
      break;

    case 'repos':
      id = 'repos-md';
      target = 'https://ceresbakalite.github.io/similarity/README.md?' + getRandomInteger(10000,1000000);
      break;

    default:
      break;

  }

  refreshMarkdown(id, target);
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
