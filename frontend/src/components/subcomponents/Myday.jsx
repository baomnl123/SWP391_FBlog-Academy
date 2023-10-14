import { useRef, useState, useEffect } from "react";

export default function Myday() {
    const [isScrollEnd, setIsScrollEnd] = useState(false)
    const container = useRef()
    const btnNext = useRef()
    const btnPrev = useRef()

    useEffect(() => {
        if (isScrollEnd) {
            container.current.scrollTo(500, 0)
            btnNext.current.classList.add('active')
            if (btnPrev.current.classList.contains('active')) {
                btnPrev.current.classList.remove('active')
            }
        }
        else {
            container.current.scrollTo(0, 0)
            btnPrev.current.classList.add('active')
            if (btnNext.current.classList.contains('active')) {
                btnNext.current.classList.remove('active')
            }
        }
    }, [isScrollEnd])


    useEffect(() => {
        function handleScroll() {
            const currentScrollWidth = container.current.scrollLeft;

            if (currentScrollWidth > 200) {
                btnNext.current.classList.add('active')
                if (btnPrev.current.classList.contains('active')) {
                    btnPrev.current.classList.remove('active')
                }
            } else {
                btnPrev.current.classList.add('active')
                if (btnNext.current.classList.contains('active')) {
                    btnNext.current.classList.remove('active')
                }
            }
        }

        container.current.addEventListener("scroll", handleScroll);

        return () => {
            container.current.removeEventListener("scroll", handleScroll);
        };
    }, []);

    function scroll() {
        setIsScrollEnd(mode => !mode)
    }

    return (
        <>

        </>
    )
}
