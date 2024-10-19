export interface AdminDevice {
    id: string;
    lastSeen: number;
    ipAddress: string;
    details: string;
    expires: number;
    userId: string;
}
