import { useEffect, useRef } from "react";

export default function Loader({ onLoaderFinished }) {
  const preLoader = useRef();

  useEffect(() => {
    const timeout = setTimeout(() => {
      preLoader.current.classList.add("hide-load");
      onLoaderFinished();
    }, 1000);

    return () => {
      clearTimeout(timeout);
    };
  }, []);

  return (
    <section className="flex-column loading" ref={preLoader}>
      <img src="images/logo.gif" alt="" />
      <div className="meta">
        <img src="images/logof.png" alt="" />
        <p>Logging in....</p>
        <div>
          <p>Chuyện gì khó lên Fblog nhó.</p>
          <p>Chia sẻ câu chuyện của bạn lên đây cùng FBLOG nhá.</p>
        </div>
      </div>
    </section>
  );
}
