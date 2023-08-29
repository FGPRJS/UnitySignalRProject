import React from "react";
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import { MainPage } from "./pages/MainPage";
import { GameServerManager } from "./pages/GameServerManager";

export function App() : React.ReactElement{
    return (
      <Tabs>
        <TabList>
          {
            Object.entries(Pages).map(page => {
              return (
                <Tab>{page[0]}</Tab>
              )
            })
          }
        </TabList>
        {
          Object.entries(Pages).map(page => {
            return (
              <TabPanel>{page[1]}</TabPanel>
            )
          })
        }
      </Tabs>
    )
}

const Pages = {
  MainPage : <MainPage></MainPage>,
  GameServerManager : <GameServerManager></GameServerManager>
}