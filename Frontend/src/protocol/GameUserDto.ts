export interface GameUserDto{
  userId : string,
  nickname : string,

  spawnToken : number,
  spawnTime : number,

  positionString : string | undefined | null
  bodyRotationString : string | undefined | null
  headRotationString : string | undefined | null
  cannonRotationString : string | undefined | null
}