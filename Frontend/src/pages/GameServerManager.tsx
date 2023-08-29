import React, { useEffect, useState } from "react";
import { GameUserDto } from "../protocol/GameUserDto";
import dayjs, {Dayjs} from "dayjs";

export function GameServerManager() : React.ReactElement{

    const [users, setUsers] = useState<GameUserDto[]>([]);

    useEffect(() => {
        async function Initialize(){
            let response = await fetch("/gameserverstatus/users",{
                method : "GET"
            });
            
            let result = await response.json();

            let users = result as GameUserDto[];

            if(users){
              setUsers(users);
            }
        }

        Initialize();
    }, [])

    return (
        <div>
            <h1>SERVER MANAGER</h1>
            <div>
                <h2>Current Users</h2>
                {
                  users.length > 0 
                  ? <div>
                      <div>Current User Count : {users.length}</div>
                      {
                        users.map((user, index) => {
                          return (
                            <div>
                              <hr/>
                              <div>
                                <h3>Base</h3>
                                <div>
                                  <div>USER ID : {user.userId}</div>
                                  <div>NICKNAME : {user.nickname}</div>
                                  <div>SPAWN TIME : {dayjs(user.spawnTime).format()}</div>
                                </div>
                              </div>
                              <div>
                                <h3>Details</h3>
                                <div>
                                  <div>POSITION : [{user.positionString}]</div>
                                  <div>BODY ROTATION : [{user.bodyRotationString}]</div>
                                  <div>HEAD ROTATION : [{user.headRotationString}]</div>
                                  <div>CANNON ROTATION : [{user.cannonRotationString}]</div>
                                </div>
                              </div>
                              <hr/>
                            </div>
                          )
                        })
                      }
                    </div> 
                  : <div>There is no user right now...</div>
                }
            </div>
        </div>
    )
}