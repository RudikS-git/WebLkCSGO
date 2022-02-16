// Узнаем ширину скролл панели
const div = document.createElement('div');
div.style.overflowY = 'scroll';
div.style.width = '50px';
div.style.height = '50px';
div.style.visibility = 'hidden';
document.body.appendChild(div);
const scrollWidth = div.offsetWidth - div.clientWidth;
document.body.removeChild(div);

const body = document.getElementsByTagName("body")[0];
let bodyScrollTop = null;
let locked = false;

export const lockScroll = () => {
  if (!locked) {
    if (body.offsetHeight < body.scrollHeight) {
      //console.log(scrollWidth);
      body.style.paddingRight = scrollWidth + 'px';
    }
    bodyScrollTop =
      typeof window.pageYOffset !== "undefined"
        ? window.pageYOffset
        : (document.documentElement ||
            document.body.parentNode ||
            document.body).scrollTop;
            
    body.classList.add("scroll-locked");
    body.style.top = `-${bodyScrollTop}px`;
    locked = true;
  }
};

export const unlockScroll = () => {
  if (locked) {
    body.classList.remove("scroll-locked");
    body.style.top = null;
    window.scrollTo(0, bodyScrollTop);
    body.style.paddingRight = '';
    locked = false;
  }
};