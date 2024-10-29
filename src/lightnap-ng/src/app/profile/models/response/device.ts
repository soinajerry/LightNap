/**
 * Interface representing a device.
 */
export interface Device {
    /**
     * The unique identifier for the device.
     * @type {string}
     */
    id: string;

    /**
     * The timestamp of the last time the device was seen.
     * @type {number}
     */
    lastSeen: number;

    /**
     * The IP address from the last time the device was seen.
     * @type {string}
     */
    ipAddress: string;

    /**
     * Device details, such as the browser agent, so it can be more easily identified by the user.
     * @type {string}
     */
    details: string;
}
