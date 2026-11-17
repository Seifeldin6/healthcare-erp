import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/consultation/chat_mockup_header.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/consultation/chat_mockup_messages.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/consultation/incoming_video_call_card.dart';

class ChatVideoMockupCard extends StatelessWidget {
  const ChatVideoMockupCard({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      width: double.infinity,
      padding: EdgeInsetsDirectional.all(AppDimensions.spacing16.w),
      decoration: BoxDecoration(
        color: AppColors.primaryTint,
        borderRadius: BorderRadius.circular(28.r),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          const ChatMockupHeader(),
          SizedBox(height: AppDimensions.spacing12.h),
          const ChatMockupMessages(),
          SizedBox(height: 14.h),
          const IncomingVideoCallCard(),
        ],
      ),
    );
  }
}