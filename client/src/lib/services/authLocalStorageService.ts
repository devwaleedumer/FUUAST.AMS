const setRefreshTokenDateTime = (refreshTokenExpiryTime: string) => localStorage.setItem("rt_dt", refreshTokenExpiryTime)
const getRefreshTokenDateTime = () => localStorage.getItem("rt_dt");
const removeRefreshTokenDateTime = () => localStorage.removeItem("rt_dt")
export { setRefreshTokenDateTime, getRefreshTokenDateTime, removeRefreshTokenDateTime }