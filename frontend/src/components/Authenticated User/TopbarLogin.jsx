import { useEffect, useRef, useState } from "react";

import { UserButton } from "@clerk/clerk-react";
export default function TopbarLogin() {
  const [isOpen, setIsOpen] = useState(false);
  const container = useRef();

  useEffect(() => {
    container.current.classList.toggle("block");
  }, [isOpen]);

  useEffect(() => {
    function handleScroll() {
      container.current.classList.remove("block");
    }
    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  function handleClick() {
    setIsOpen((open) => !open);
  }

  return (
    <header>
      <nav>
        <div className="logo">
          <a href="/">
            <img src="images/logo.png" alt="" />
          </a>
        </div>
        <div className="search hover">
          <i className="fa-sharp fa-solid fa-magnifying-glass search_icon"></i>
          <input type="text" placeholder="Search FBLOG" />
        </div>
        <div className="top-icon hover toggle-menu">
          <a href="#!">
            <img src="images/bars.png" alt="" title="Menu" />
          </a>
        </div>
        <ul className="top-menu">
          <UserButton />
          <li className="nav" onClick={handleClick} ref={container}></li>
        </ul>
      </nav>
    </header>
  );
}
