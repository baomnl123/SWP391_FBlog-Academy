//import Groups from "./subcomponents/Groups";

export default function Leftbar() {
    return (
        <nav className="left-bar">

            <div className="signal"></div>

            <div className="flex-item">
                <div>
                    <a href="#!" className="side-icon hover2">
                        <i className="fa-solid fa-house"></i>
                        <font>Home</font>
                    </a>
                </div>
                <div>
                    <a href="#!" className="side-icon2 hover2">
                        <div className="circle-container2">
                            <img src="images/news.png" alt="" />
                        </div>
                        <font>Post Blog</font>
                    </a>
                </div>
            </div>

            <div className="flex-item">
                <div>
                    <a href="#!" className="side-icon2 hover2">
                        <div className="circle-container">
                            <img src="images/group.png" alt="" />
                        </div>
                        <font>Follwing</font>
                    </a>
                </div>
                <div>
                    <a href="#!" className="side-icon2 hover2">
                        <div className="circle-container2">
                            <img src="images/watch.png" alt="" />
                        </div>
                        <font>Blog have Video</font>
                    </a>
                </div>

                <div>
                    <a href="#!" className="side-icon2 hover2">
                        <div className="circle-container2">
                            <img src="images/gaming.png" alt="" />
                        </div>
                        <font>Blog have picture</font>
                    </a>
                </div>
            </div>



            <div className="flex-item">
                <div>
                    <a href="#!" className="side-icon2 hover2">
                        <div className="circle-container">
                            <img src="images/link.png" alt="" />
                        </div>
                        <font>Shortcuts</font>
                    </a>
                </div>
            </div>




        </nav>
    )
}
