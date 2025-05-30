/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
    remotePatterns: [{
         protocol: 'https',
        hostname: 'localhost',
        port: '7081',
        pathname: '/assets/images/**',
    }],
    }
};

export default nextConfig;
