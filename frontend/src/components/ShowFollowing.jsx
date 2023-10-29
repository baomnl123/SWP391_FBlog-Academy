import listUsers from "../List/ListUser";
import { useState } from "react";

import Follwing from "./Following";

const ShowFollwing = () => {
  const [ListUsers] = useState(listUsers);

  return <Follwing user={ListUsers} />;
};
export default ShowFollwing;
