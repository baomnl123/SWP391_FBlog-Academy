import { useEffect, useRef, useState } from "react"

export default function Topbar() {
    const [isOpen, setIsOpen] = useState(false)
    const container = useRef()

    useEffect(() => {
        container.current.classList.toggle('block')
    }, [isOpen])

    useEffect(() => {
        function handleScroll() {
            container.current.classList.remove('block')
        }
        window.addEventListener('scroll', handleScroll)

        return () => {
            window.removeEventListener('scroll', handleScroll)
        }
    }, [])

    function handleClick() {
        setIsOpen(open => !open)
    }

    return (
        <header>
            <nav>
                <div className="logo">
                    <a href=""><img src="images/logo.png" alt="" /></a>
                </div>
                <div className="search hover">
                    <i className="fa-sharp fa-solid fa-magnifying-glass search_icon"></i>
                    <input type="text" placeholder="Search FBLOG" />
                </div>
                <div className="top-icon hover toggle-menu">
                    <a href="#!"><img src="images/bars.png" alt="" title="Menu" /></a>
                </div>
                <ul className="top-menu">

                    <li className="top-icon profile-nav" onClick={handleClick} ref={container}>
                        <a><img src="images/robin.png" alt="" /></a>
                        <ul className="new">
                            <h3> What's New?    </h3>
                            <li>• Profile</li>
                            <li>• Log out</li>
                        </ul>
                    </li>
                </ul>
            </nav>
            <ul className="nav-menu flex-row">
                <li><a href="#!"><i className="fa-sharp fa-solid fa-house"></i></a></li>
                <li><a href="#!"><img src="images/friend-mobile.png" alt="" /></a></li>
                <li><a href="#!"><img src="images/mes-mobile.png" alt="" /></a></li>
                <li><a href="#!"><i className="fa-regular fa-bell"></i></a></li>
                <li><a href="#!"><i className="bx bx-slideshow"></i></a></li>
                <li><a href="#!"><i className="bx bx-store"></i></a></li>
            </ul>
        </header>
    )
}
